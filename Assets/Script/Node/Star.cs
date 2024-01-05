using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private ParticleSystem particleSystem;
    private bool isLight = false;

    // Start is called before the first frame update
    private void Awake()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        particleSystem = transform.GetChild(3).GetComponentInChildren<ParticleSystem>();
    }

    void Start()
    {
        UnlightStar();
        animator.Play("Star");
    }

    private void Update()
    {
        if (!isLight)
        {
            WaysManager.instance.CheckStar(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Star") && isLight && !GameManager.instance.IsGameWin())
        {
            particleSystem.Play();
            this.GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            WaysManager.instance.DestroyPoints();
            GameManager.instance.Win();
        }
    }

    public void LightStar()
    {
        spriteRenderer.DOFade(1f, 1f);
        isLight = true;
    }

    public void UnlightStar()
    {
        spriteRenderer.DOFade(0f, 1f);
        isLight = false;
    }
}
