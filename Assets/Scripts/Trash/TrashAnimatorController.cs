using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashAnimatorController : MonoBehaviour
{
    Animator anim;
    TrashController controller;
    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        if (controller == null)
        {
            controller = GetComponentInParent<TrashController>();
            controller.SendControllerToAnimatorEvent += SendControllerToAnimatorEventReceiver;
        }
    }

    private void OnDisable()
    {
        if (controller != null)
        {
            controller.SendControllerToAnimatorEvent -= SendControllerToAnimatorEventReceiver;
        }
    }


    private void SendControllerToAnimatorEventReceiver(RuntimeAnimatorController obj)
    {
        Debug.Log($"{this.name} fired animation!");
        if (!gameObject.activeInHierarchy) return;
        if (anim == null) return;
        anim.runtimeAnimatorController = obj;
        int stateHash = Animator.StringToHash("New State");
        anim.enabled = false;
        anim.enabled = true;
        anim.Rebind();
        anim.Update(0f);
        anim.Play(stateHash, 0, 0f);
    }
}
