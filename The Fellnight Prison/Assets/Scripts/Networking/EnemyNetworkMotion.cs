using UnityEngine;
using System.Collections;

public class EnemyNetworkMotion : Photon.MonoBehaviour
{

    Vector3 realPosition = Vector3.zero;
    Quaternion realRotation = Quaternion.identity;
    public PhotonView myView;

    Animator anim;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();

        myView = this.gameObject.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.isMine)
        {

        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, realPosition, 0.1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 0.1f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(anim.GetBool("isWalking"));
            stream.SendNext(anim.GetBool("isAttaking"));

        }
        else
        {
            realPosition = (Vector3)stream.ReceiveNext();
            realRotation = (Quaternion)stream.ReceiveNext();
            anim.SetBool("isWalking", (bool)stream.ReceiveNext());
            anim.SetBool("isAttacking", (bool)stream.ReceiveNext());
        }
    }

}