using System.Collections;
using Narration;
using UnityEngine;

public class PortalNarrationTrigger : MonoBehaviour
{
    [SerializeField] GameObject portalPrefab;
    [SerializeField] float portalHeightAbovePlatform = 3f;
    bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;
        float audioLength = GetComponent<NarrationTrigger>().audioClip.length;
        StartCoroutine(SpawnPortalAfterDelay(audioLength));
        triggered = true;
    }

    IEnumerator SpawnPortalAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject portalGO = Instantiate(portalPrefab);
        int nextLevelPieceIndexToShowWordOn = FindAnyObjectByType<SubtitlePlayer>().nextLevelPieceIndexToShowWordOn;
        //find that level piece
        GameObject levelPiece = FindObjectOfType<LevelGenerator>().levelPiecesSpawned[nextLevelPieceIndexToShowWordOn];
        //move the portal to be right right above it
        Vector3 portalPos = levelPiece.transform.position + Vector3.up * portalHeightAbovePlatform;
        portalGO.transform.position = portalPos;
        //rotate it towards the player
        portalGO.transform.forward = GameObject.FindWithTag("Player").transform.position - portalGO.transform.position;

        //also, set the portals destination to the machine platform.
        Transform socketOnMachine = FindObjectOfType<ChrystalInserter>().transform;
        Vector3 socketPos = socketOnMachine.position;
        Vector3 positionOnMachinePlatform = socketPos + (Vector3.back * 2f);
        Portal portal = portalGO.GetComponent<Portal>();
        portal.destination = positionOnMachinePlatform;

        //and maybe the portal should have the ability to rotate the player, set the rotation to face the machine, so it makes sense.
        Vector3 dirFromMachinPosToSocket = socketPos - positionOnMachinePlatform;
        portal.forwardDirAfterTeleport = dirFromMachinPosToSocket;

    }
}
