using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private PlayerController playerController;
    private Rigidbody rb;
    private WeaponManager wm;
    private Collider playerCollider;
    private EntityStats playerStats;


    private void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
        rb = player.GetComponent<Rigidbody>();
        wm = player.GetComponent<WeaponManager>();
        playerCollider = player.GetComponent<Collider>();
        playerStats = player.GetComponent<EntityStats>();
    }

    public void PreparePlayer()
    {
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        player.transform.position = transform.position;
        yield return new WaitForSeconds(0.1f);

        player.gameObject.layer = (int)Layer.Player;

        wm.enabled = true;
        playerCollider.enabled = true;

        playerStats.EnablePlayer();
        playerStats.SetFullHealth();
    }
}
