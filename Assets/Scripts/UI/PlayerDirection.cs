using System.Collections.Generic;
using UnityEngine;

public class PlayerDirection : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private List<GameObject> EnemyLocations = new List<GameObject>();
    [SerializeField] private float hideDistance = 3f; // Khoảng cách đủ gần để tự động ẩn line
    public static PlayerDirection instance;
    private int activeIndex = -1;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
    }
    private void Update()
    {
        if (lineRenderer != null && lineRenderer.enabled && activeIndex >= 0 && activeIndex < EnemyLocations.Count)
        {
            if (EnemyLocations[activeIndex] != null)
            {
                Vector3 playerPos = transform.position;
                Vector3 targetPos = EnemyLocations[activeIndex].transform.position;
                if (Vector3.Distance(playerPos, targetPos) <= hideDistance)
                {
                    HideDirection();
                    return;
                }
                lineRenderer.SetPosition(1, playerPos);
                lineRenderer.SetPosition(0, targetPos);
            }
            else
            {
                HideDirection();
            }
        }
    }
    public void ShowDirection(int index)
    {
        if (lineRenderer == null) return;
        if (index >= 0 && index < EnemyLocations.Count && EnemyLocations[index] != null)
        {
            activeIndex = index;
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(1, transform.position);
            lineRenderer.SetPosition(0, EnemyLocations[activeIndex].transform.position);
        }
        else
        {
            HideDirection();
        }
    }
    public void HideDirection()
    {
        activeIndex = -1;
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
    }
}