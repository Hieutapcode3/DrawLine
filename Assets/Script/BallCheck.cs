using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallCheck : MonoBehaviour
{
    [SerializeField] private GameObject failPanel;
    [SerializeField] private GameObject successPanel;
    [SerializeField] private GameObject endGamePanel;
    private Rigidbody2D rb;
    private void Start() {
        rb =GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        Debug.Log(SceneManager.GetActiveScene().buildIndex + " " + SceneManager.sceneCountInBuildSettings);
    }
    private void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.CompareTag("Ground")){
            StartCoroutine(ActiveFailPanel());
        }
    }
    private void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.CompareTag("Gem")){
            Destroy(col.gameObject);
            if((SceneManager.GetActiveScene().buildIndex + 1 == SceneManager.sceneCountInBuildSettings) && endGamePanel != null)
                StartCoroutine(ActiveEndGamePanel());
            else
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
    private IEnumerator ActiveEndGamePanel()
    {
        yield return new WaitForSeconds(0.5f);
        endGamePanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void StartGame(){
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
}
