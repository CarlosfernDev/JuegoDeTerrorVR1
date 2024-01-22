using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemigoDeLuzRemake : MonoBehaviour
{
    public static EnemigoDeLuzRemake instance;

    private float TimeSlider;
    public float _lessSlider;
    public float _addSlider;
    public float coldownReduce;
    public float coldownAdd;

    public Slider _slider;

    public GameObject prefabObjetoAMover;
    public GameObject posicionInicial;

    public GameObject[] posiciones;
    private int enemyPosition;

    public Light mylight;
    private float IntesityStart;

    public Coroutine CoreCorutine;

    // Start is called before the first frame update
    void Awake()
    {
        _slider.value = 1;
        instance = this;
        IntesityStart = mylight.intensity;
    }

    public void StarTimer()
    {
        TimeSlider = 1;
        enemyPosition = posiciones.Length - 1;
        prefabObjetoAMover.transform.position = posicionInicial.transform.position;
        _slider.value = TimeSlider;

        CoreCorutine = StartCoroutine(TimeReduce());
    }

    public void RestartTimer()
    {
        StopCoroutine(CoreCorutine);
        CoreCorutine = StartCoroutine(TimeReduce());
    }

    public void PauseTimer()
    {
        StopCoroutine(CoreCorutine);
        CoreCorutine = StartCoroutine(TimeAdd());
    }

    IEnumerator TimeReduce()
    {
        while (true)
        {

            TimeSlider -= _lessSlider;

            SetSliderValue();
            TestTime();
            

            MoveEnemy();

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator TimeAdd()
    {
        yield return new WaitForSeconds(coldownAdd);
        while (true)
        {
            AddValor();
            SetSliderValue();

            MoveEnemy();

            yield return new WaitForSeconds(coldownAdd);
        }
    }

    public void TestTime()
    {
        if (TimeSlider > 0)
            return;

        Debug.Log("Te mato");
        StopCoroutine(CoreCorutine);
        GameManager.Instance.Lose();
    }

    /////
    
    private void MoveEnemy()
    {
        Debug.Log(TimeSlider / (1f / (float)posiciones.Length));

        int newEnemyPosition = (int)Mathf.Floor(TimeSlider / (1f / (float)posiciones.Length));

        newEnemyPosition = Mathf.Clamp(newEnemyPosition - 1, 0 , posiciones.Length - 1);

        if (newEnemyPosition == enemyPosition)
            return;

        Debug.Log(newEnemyPosition);
        prefabObjetoAMover.transform.position = posiciones[newEnemyPosition].transform.position;
        prefabObjetoAMover.transform.rotation = posiciones[newEnemyPosition].transform.rotation;
        enemyPosition = newEnemyPosition;
    }

    private void SetSliderValue()
    {
        _slider.value = TimeSlider;
        mylight.intensity = Mathf.Lerp(0, IntesityStart, TimeSlider);
    }

    public void AddValor()
    {
        TimeSlider += _addSlider;
    }

}
