using UnityEngine;
using System.Collections;

public class NetworkMotion : Photon.MonoBehaviour
{

    Vector3 realPosition = Vector3.zero;
    Quaternion realRotation = Quaternion.identity;

    Animator anim;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
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
            stream.SendNext(anim.GetFloat("posX"));
            stream.SendNext(anim.GetFloat("Ydirection"));
            stream.SendNext(anim.GetFloat("Run"));
            stream.SendNext(anim.GetFloat("inputH"));
            stream.SendNext(anim.GetFloat("inputV"));
            stream.SendNext(anim.GetBool("isRunning"));
            stream.SendNext(anim.GetBool("isBlocking"));

        }
        else
        {
            realPosition = (Vector3)stream.ReceiveNext();
            realRotation = (Quaternion)stream.ReceiveNext();
            anim.SetFloat("posX", (float)stream.ReceiveNext());
            anim.SetFloat("Ydirection", (float)stream.ReceiveNext());
            anim.SetFloat("Run", (float)stream.ReceiveNext());
            anim.SetFloat("inputH", (float)stream.ReceiveNext());
            anim.SetFloat("inputV", (float)stream.ReceiveNext());
            anim.SetBool("isRunning", (bool)stream.ReceiveNext());
            anim.SetBool("isBlocking", (bool)stream.ReceiveNext());

        }
    }
}