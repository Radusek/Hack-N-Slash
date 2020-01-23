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

    private GameObject pointedEntity;
    private EntityStats pointedEntityStats;

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

            if (pointedEntityStats != null)
                RemoveEventListeners();
            SetNewTarget(hit);
        }
        else if (pointedEntity != null)
        {
            if (pointedEntity.layer != (int)Layer.Enemy && pointedEntity.layer != (int)Layer.Enemy2)
            {
                pointedEntity = null;
                tooltipObject.SetActive(false);
                return;
            }

            if (cancelTargetCoroutine == null)
                cancelTargetCoroutine = StartCoroutine(CancelCurrentTargetCoroutine());
        }
    }

    private void SetNewTarget(RaycastHit newTargetHit)
    {
        pointedEntity = newTargetHit.collider.gameObject;
        nameText.text = pointedEntity.name;

        int targetLayer = newTargetHit.collider.gameObject.layer;
        if (targetLayer == (int)Layer.Enemy || targetLayer == (int)Layer.Enemy2)
        {
            SetEnemyObjects(true);
            pointedEntityStats = newTargetHit.collider.gameObject.GetComponent<EntityStats>();

            UpdateHpBar();
            UpdateLevel();

            pointedEntityStats.OnHpChanged.AddListener(UpdateHpBar);
            pointedEntityStats.OnLevelUp.AddListener(UpdateLevel);
        }
        else
            SetEnemyObjects(false);
    }

    private void RemoveEventListeners()
    {
        pointedEntityStats.OnHpChanged.RemoveListener(UpdateHpBar);
        pointedEntityStats.OnLevelUp.RemoveListener(UpdateLevel);
    }

    private void UpdateHpBar()
    {
        hpBar.value = pointedEntityStats.GetHpFraction();
        Vector2Int hpValues = pointedEntityStats.GetHpValues();
        if (hpValues.x < 0)
            hpValues.x = 0;
        healthNumbers.text = hpValues.x.ToString() + "/" + hpValues.y.ToString();
    }

    private void UpdateLevel()
    {
        levelText.text = pointedEntityStats.GetLevel().ToString();
    }

    private IEnumerator CancelCurrentTargetCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        RemoveEventListeners();
        pointedEntityStats = null;
        pointedEntity = null;
        tooltipObject.SetActive(false);
        cancelTargetCoroutine = null;
    }

    private void SetEnemyObjects(bool val)
    {
        levelText.gameObject.SetActive(val);
        hpBar.gameObject.SetActive(val);
    }
}
