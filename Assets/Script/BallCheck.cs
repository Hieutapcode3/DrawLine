using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCheck : MonoBehaviour
{
    [SerializeField] private GameObject failPanel;
    [SerializeField] private GameObject successPanel;
    private Rigidbody2D rb;
    private void Start() {
        rb =GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
    }
    private void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.CompareTag("Ground")){
            StartCoroutine(ActiveFailPanel());
        }
    }
    private void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.CompareTag("Gem")){
            Destroy(col.gameObject);
            StartCoroutine(ActiveSuccessPanel());
        }
    }
    private IEnumerator ActiveFailPanel(){
        yield return new WaitForSeconds(0.5f);
        failPanel.SetActive(true);
        Time.timeScale = 0;
    }
    private IEnumerator ActiveSuccessPanel(){
        yield return new WaitForSeconds(0.5f);
        successPanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void StartGame(){
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
}
