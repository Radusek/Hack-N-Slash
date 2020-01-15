using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipHandler : MonoBehaviour
{
    [SerializeField]
    private Camera camera;

    [SerializeField]
    private LayerMask raycastMask;

    private EntityStats pointedEntity;

    [SerializeField]
    private GameObject tooltipObject;
    [SerializeField]
    private Slider hpBar;
    [SerializeField]
    private TextMeshProUGUI healthNumbers;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private TextMeshProUGUI nameText;


    private Coroutine cancelTargetCoroutine;


    void Update()
    {
        CheckForObject();


    }

    private void CheckForObject()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastMask))
        {
            if (pointedEntity == null)
            {
                SetNewTarget(hit);
                tooltipObject.SetActive(true);
                return;
            }

            if (hit.collider.gameObject == pointedEntity.gameObject)
                return;

            if (cancelTargetCoroutine != null)
            {
                StopCoroutine(cancelTargetCoroutine);
                cancelTargetCoroutine = null;
            }

            RemoveEventListeners();
            SetNewTarget(hit);
        }
        else if (pointedEntity != null)
        {
            if (cancelTargetCoroutine == null)
                cancelTargetCoroutine = StartCoroutine(CancelCurrentTargetCoroutine());
        }
    }

    private void SetNewTarget(RaycastHit newTargetHit)
    {
        pointedEntity = newTargetHit.collider.gameObject.GetComponent<EntityStats>();
        SetInitialHUDValues();

        pointedEntity.OnHpChanged.AddListener(UpdateHpBar);
        pointedEntity.OnLevelUp.AddListener(UpdateLevel);
    }

    private void RemoveEventListeners()
    {
        pointedEntity.OnHpChanged.RemoveListener(UpdateHpBar);
        pointedEntity.OnLevelUp.RemoveListener(UpdateLevel);
    }

    private void SetInitialHUDValues()
    {
        nameText.text = pointedEntity.name;
        UpdateLevel();
        UpdateHpBar();
    }

    private void UpdateHpBar()
    {
        hpBar.value = pointedEntity.GetHpFraction();
        Vector2Int hpValues = pointedEntity.GetHpValues();
        if (hpValues.x < 0)
            hpValues.x = 0;
        healthNumbers.text = hpValues.x.ToString() + "/" + hpValues.y.ToString();
    }

    private void UpdateLevel()
    {
        levelText.text = pointedEntity.GetLevel().ToString();
    }

    private IEnumerator CancelCurrentTargetCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        RemoveEventListeners();
        pointedEntity = null;
        tooltipObject.SetActive(false);
        cancelTargetCoroutine = null;
    }
}
