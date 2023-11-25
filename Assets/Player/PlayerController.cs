using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Timeline.AnimationPlayableAsset;

public class PlayerController : MonoBehaviour
{
    public UIController UI;
    public GameObject ShotHit;
    public Transform CurrentSelectedFruit;

    //2d controls
    public int LastFruit = -1;
    public int LastPair = -1;
    public float LastX;
    public float LastZ;

    public int Chain = 0;

    bool PoisonActive = false;
    public float PoisonWaitTime = 2;

    public int Score = 0;
    public float Health = 100;

    bool reloaded = true;

    ///3d Controls <summary>
    Vector2 rotation = Vector2.zero;
    const string xAxis = "Mouse X"; //Strings in direct code generate garbage, storing and re-using them creates no garbage
    const string yAxis = "Mouse Y";
    public float sensitivity = 5f;
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PoisonActive)
        {
            StartCoroutine(PoisonRoutine());
        }
        if (!transform.GetComponent<Camera>().orthographic)
        {
            rotation.x += Input.GetAxis(xAxis) * sensitivity;
            rotation.y += Input.GetAxis(yAxis) * sensitivity;
            rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
            var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
            var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);
            transform.localRotation = xQuat * yQuat;
        }
        if (Health <= 0)
        {
            UI.Die(Score);
        }
    }

    public void TakeDamage(float Damage)
    {
        Health -= Damage;
    }

    public void Eat()
    {
        Health += Chain;
        Chain = 0;
        LastFruit = -1;
        LastPair = -1;
        if (Health > 100)
        {
            Health = 100;
        }
        CurrentSelectedFruit.gameObject.SetActive(false);
    }

    void OnClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.transform.GetComponent<FruitController>() != null)
            {
                CheckNextFruit(hit.transform.GetComponent<FruitController>());
            }
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy") && reloaded)
            {
                hit.transform.GetComponent<GoblinController>().TakeDamage(5);
                Instantiate(ShotHit, hit.point, Quaternion.identity);
                StartCoroutine(ReloadRoutine());
            }
        }
    }

    void OnSwitch()
    {
        UI.SwitchButtonClicked();
    }

    void CheckNextFruit(FruitController fruit)
    {
        if (LastFruit == -1)
        {
            LastFruit = fruit.m_fruitType;
            LastPair = fruit.m_fruitPair;
            LastX = fruit.transform.position.x; LastZ = fruit.transform.position.z;
            fruit.transform.localScale *= 1.3f;
            fruit.m_clicked = true;
            CurrentSelectedFruit.position = fruit.transform.position;
            CurrentSelectedFruit.gameObject.SetActive(true);
            return;
        }
        if ((fruit.m_fruitType == LastFruit  || (fruit.m_fruitPair != -1 && fruit.m_fruitPair == LastPair)) &&  fruit.m_clicked == false)
        {
            if (Mathf.Abs(fruit.transform.position.x - LastX) <= 1 && Mathf.Abs(fruit.transform.position.z - LastZ) <= 1)
            {
                Chain++;
                Score += Chain;
                fruit.transform.localScale *= 1.5f;
                LastFruit = fruit.m_fruitType;
                LastPair = fruit.m_fruitPair;
                LastX = fruit.transform.position.x; LastZ = fruit.transform.position.z;
                CurrentSelectedFruit.position = fruit.transform.position;
                CurrentSelectedFruit.gameObject.SetActive(true);
                fruit.m_clicked = true;
            }
        }
    }

    IEnumerator PoisonRoutine()
    {
        PoisonActive = true;
        yield return new WaitForSeconds(PoisonWaitTime);
        Health -= 1;
        if (PoisonWaitTime > 0.2f)
        {
            PoisonWaitTime -= 0.02f;
        }
        PoisonActive = false;
    }

    IEnumerator ReloadRoutine()
    {
        reloaded = false;
        yield return new WaitForSeconds(0.3f);
        reloaded = true;
    }
}
