using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private const float SHOT_COOLDOWN_SECONDS = .6f;

    [SerializeField] private Animator anim;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Material decalMaterial;
    [SerializeField] private Texture decalTexture;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private GameObject onHitExplosionPrefab;
    [SerializeField] private LayerMask hittableLayerMask;
    [SerializeField] private LayerMask destructableLayerMask;

    private float cooldownEndTime = 0;

    private void Update()
    {
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) && Time.time > cooldownEndTime)
        {
            cooldownEndTime = Time.time + SHOT_COOLDOWN_SECONDS;
            anim.SetTrigger("Shoot");
            audioSource.Play();
            if (Physics.Raycast(firePoint.position, firePoint.forward, out RaycastHit hit, float.MaxValue,
                hittableLayerMask | destructableLayerMask))
            {
                if (hittableLayerMask == (hittableLayerMask | (1 << hit.collider.gameObject.layer)))
                {
                    SpawnDecal(hit);
                    Instantiate(onHitExplosionPrefab, hit.point, Quaternion.identity);
                } else if (destructableLayerMask == (destructableLayerMask | (1 << hit.collider.gameObject.layer)))
                {
                    var brick = hit.collider.gameObject.GetComponent<Brick>();
                    if (brick.IsAlive)
                        Instantiate(onHitExplosionPrefab, hit.point, Quaternion.identity);
                    brick.Destroy();
                }
                
                
            }
            particle.gameObject.SetActive(true);
            particle.Play();
            Invoke(nameof(HideShotParticle), SHOT_COOLDOWN_SECONDS - .1f);
        }
    }

    private void SpawnDecal(RaycastHit hit)
    {
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Destroy(quad.GetComponent<MeshCollider>());
        quad.name = "Decal";
        quad.AddComponent<DestroyByTime>();
        quad.transform.position = hit.point + hit.normal.normalized * .01f;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        rotation *= Quaternion.Euler(90f, 0f, 0f);
        quad.transform.rotation = rotation;
        quad.transform.localScale = new Vector3(.2f, .2f, .2f);
        MeshRenderer renderer = quad.GetComponent<MeshRenderer>();
        renderer.material = decalMaterial;
        decalMaterial.mainTexture = decalTexture;
    }

    private void HideShotParticle()
    {
        particle.gameObject.SetActive(false);
    }
}
