using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour 
{
	[SerializeField] float speed;

	public Transform recticle;

	Transform crossTop;
	Transform crossBottom;
	Transform crossLeft;
	Transform crossRight;
	float recticleStartPoint;
    float recticleAimPoint;

    void Start()
	{
		crossTop = recticle.Find ("Cross/Top");
		crossBottom = recticle.Find ("Cross/Bottom");
		crossLeft = recticle.Find ("Cross/Left");
		crossRight = recticle.Find ("Cross/Right");

		recticleStartPoint = crossTop.localPosition.y;
	}

	void Update()
	{
        recticleAimPoint = recticleStartPoint;
        if (GameManager.Instance.LocalPlayer.PlayerState.weaponState == PlayerState.EWeaponState.AIMED_FIRING || GameManager.Instance.LocalPlayer.PlayerState.weaponState == PlayerState.EWeaponState.AIMING)
            recticleAimPoint = recticleStartPoint / 2;

        Vector3 screenPosition = Camera.main.WorldToScreenPoint (transform.position);
		recticle.transform.position = Vector3.Lerp (recticle.transform.position, screenPosition, speed * Time.deltaTime);
	}

    void SetVisibility(bool value)
    {
        recticle.gameObject.SetActive(value);
    }

    public void ApplyScale(float scale)
	{
        crossTop.localPosition = new Vector3 (0, recticleAimPoint + scale, 0);
		crossBottom.localPosition = new Vector3 (0, -recticleAimPoint - scale, 0);
		crossLeft.localPosition = new Vector3 (-recticleAimPoint - scale, 0, 0);
		crossRight.localPosition = new Vector3 (recticleAimPoint + scale, 0, 0);
	}
}
