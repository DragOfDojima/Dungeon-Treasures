using Photon.Pun;
using UnityEngine;
using System.Collections;

public class RoomAnchorManager : MonoBehaviourPun
{
    public Transform roomAnchor; // Assign manually or find by name

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => roomAnchor != null && roomAnchor.gameObject.activeInHierarchy);

        Debug.Log("✅ Anchor found in scene.");

        yield return new WaitUntil(() => PhotonNetwork.InRoom && photonView.IsMine);

        yield return new WaitForSeconds(1f);

        Debug.Log("✅ Broadcasting anchor position...");
        BroadcastAnchor();
    }

    void BroadcastAnchor()
    {
        photonView.RPC("SyncAnchor", RpcTarget.OthersBuffered, roomAnchor.position, roomAnchor.rotation);
    }

    [PunRPC]
    void SyncAnchor(Vector3 pos, Quaternion rot)
    {
        roomAnchor.position = pos;
        roomAnchor.rotation = rot;
    }
}
