using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrashController : MonoBehaviour
{
    [SerializeField] float spd;
    [SerializeField] float maxSpd;
    [SerializeField] int trashType;
    [SerializeField] List<GameObject> listOfTypes = new List<GameObject>();
    [SerializeField] KeyCode currentKey;
    [SerializeField] RuntimeAnimatorController idleController;
    [SerializeField] RuntimeAnimatorController destroyedController;
    public KeyCode GetCurrentKeyCode() => currentKey;
    LayerMask trashLayer;
    public Collider2D trashCollider;
    public Vector3 colliderSize;
    TrashSpawnerGameObject spawner;


    private void OnEnable()
    {
        if (spawner == null) { spawner = FindFirstObjectByType<TrashSpawnerGameObject>(); }
        trashCollider = transform.GetComponent<Collider2D>();
        colliderSize = trashCollider.bounds.size;
        trashLayer = LayerMask.GetMask("Trashes");
        ChangeType();
        ChangePosition();
    }


    private void Update()
    {
        transform.position += Vector3.down * spd * Time.deltaTime;

        if (transform.position.y <= -4.45f)
        {
            GameManager.instance.OnSortCorrect(false, trashType);
            ChangeType();
            ChangePosition();
        }

        if (GameManager.instance.GetStartSpawningTrashCO() == null)
        {
            if (spd <= maxSpd) { spd += Time.deltaTime * 0.085f; }
        }
        else
        {
            spd = 2;
        }
    }


    private void ChangePosition()
    {
        Vector3 newPos = spawner.GetTransform().GetChild(Random.Range(0, spawner.GetTransform().childCount)).position;
        Vector3 halfExtents = colliderSize / 2;
        int maxTries = 10;
        float spaceBuffer = 2f;

        for (int i = 0; i < maxTries; i++)
        {
            Collider2D[] colliders = new Collider2D[5];
            int numOfCollider = Physics2D.OverlapBoxNonAlloc(newPos, halfExtents, transform.eulerAngles.z, colliders, trashLayer);
            if (numOfCollider < 1) { transform.position = newPos; return; }

            float maxOverlapSize = 0f;
            foreach (Collider2D col in colliders)
            {
                if (col == trashCollider || col == null) continue;
                Vector3 overlapColliderSize = col.bounds.size;
                maxOverlapSize = Mathf.Max(maxOverlapSize, Mathf.Max(overlapColliderSize.x, overlapColliderSize.y));
            }
            float stepDistance = (Mathf.Max(colliderSize.x, colliderSize.y) + maxOverlapSize) / 2f + spaceBuffer;
            newPos += Vector3.up * stepDistance;
        }
        transform.position = newPos;
    }


    private void ChangeType()
    {
        int newType = Random.Range(0, TrashTypes.TrashKeys.Count);
        while (newType == trashType)
        {
            newType = Random.Range(0, TrashTypes.TrashKeys.Count);
        }
        if (newType > listOfTypes.Count) return;
        if (newType > TrashTypes.TrashKeys.Count) return;
        listOfTypes[trashType].SetActive(false);
        trashType = newType;
        KeyCode nextCode = KeyCode.None;
        if (TrashTypes.TrashKeys.TryGetValue(newType, out nextCode)) currentKey = nextCode;
        listOfTypes[trashType].SetActive(true);
    }


    Coroutine CO_DestroyedAnimation;
    IEnumerator DestroyedAnimation()
    {
        var flt_Count = 0f;
        var flt_TimeMax = 0.26f;

        SendControllerToAnimatorEvent?.Invoke(destroyedController);
        while (flt_Count < flt_TimeMax)
        {
            flt_Count += Time.deltaTime;
            Debug.Log(flt_Count);
            yield return null;
        }

        CO_DestroyedAnimation = null;
        SendControllerToAnimatorEvent?.Invoke(idleController);
        ChangeType();
        ChangePosition();
    }


    public event Action<RuntimeAnimatorController> SendControllerToAnimatorEvent;
    public void ActivateSort(bool isCorrect)
    {
        Debug.Log(isCorrect);
        GameManager.instance.OnSortCorrect(isCorrect, trashType);
        if (CO_DestroyedAnimation != null) return;

        CO_DestroyedAnimation = StartCoroutine(DestroyedAnimation());
    }
}
