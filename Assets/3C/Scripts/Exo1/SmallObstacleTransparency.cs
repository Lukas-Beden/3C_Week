using UnityEngine;

public class SmallObstacleTransparency_SimpleURP : MonoBehaviour
{
    public Transform target;
    public LayerMask obstacleLayers;
    public float transparentAlpha = 0.3f;
    public float fadeSpeed = 8f;

    Renderer lastObstacle;

    void LateUpdate()
    {
        if (!target) return;

        Vector3 camPos = Camera.main.transform.position;
        Vector3 dir = target.position - camPos;

        if (Physics.Raycast(camPos, dir, out RaycastHit hit, dir.magnitude, obstacleLayers))
        {
            Renderer r = hit.collider.GetComponent<Renderer>();
            if (!r) r = hit.collider.GetComponentInChildren<Renderer>();

            if (r && r != lastObstacle)
            {
                RestoreLast();
                lastObstacle = r;
                SetTransparent(r);
            }
        }
        else
        {
            RestoreLast();
        }
    }



    void SetTransparent(Renderer r)
    {
        foreach (var m in r.materials)
        {
            SetURPTransparent(m);
        }
    }

    void RestoreLast()
    {
        if (lastObstacle)
        {
            foreach (var m in lastObstacle.materials)
            {
                SetURPOpaque(m);
            }
            lastObstacle = null;
        }
    }



    void SetURPTransparent(Material mat)
    {
        if (!mat.HasProperty("_Surface")) return;
        mat.SetFloat("_Surface", 1);
        mat.SetFloat("_SrcBlend", 1);
        mat.SetFloat("_DstBlend", 10);
        mat.SetFloat("_ZWrite", 0);
        mat.renderQueue = 3000;

        Color c = mat.color;
        c.a = transparentAlpha;
        mat.color = c;
    }

    void SetURPOpaque(Material mat)
    {
        if (!mat.HasProperty("_Surface")) return;
        mat.SetFloat("_Surface", 0);
        mat.SetFloat("_SrcBlend", 1);
        mat.SetFloat("_DstBlend", 0);
        mat.SetFloat("_ZWrite", 1);
        mat.renderQueue = 2000;

        Color c = mat.color;
        c.a = 1f;
        mat.color = c;
    }
}
