using UnityEngine;
using static ActivateTrigger;

[RequireComponent(typeof(Collider2D))]
public class ActivateTrigger : MonoBehaviour
{
    public enum Action { None, Activate, Deactivate, Toggle, Destroy };

    public ActivateTriggerTargetData[] targets;
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PerformActions(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PerformActions(false);
        }
    }

    private void PerformActions(bool isEnter)
    {
        foreach (var targetData in targets)
        {
            StartCoroutine(PerformAction(isEnter, targetData));
        }
    }

    private System.Collections.IEnumerator PerformAction(bool isEnter, ActivateTriggerTargetData data)
    {
        if (data.target != null)
        {
            yield return new WaitForSeconds(data.delay);

            switch (isEnter ? data.onEnterAction : data.onExitAction)
            {
                case Action.Activate:
                    SetState(data.target, true);
                    break;
                case Action.Deactivate:
                    SetState(data.target, false);
                    break;
                case Action.Toggle:
                    SetState(data.target, !GetState(data.target));
                    break;
                case Action.Destroy:
                    Destroy(data.target);
                    break;
            }
        }
    }

    private void SetState(Object obj, bool value)
    {
        if (obj is MonoBehaviour)
        {
            ((MonoBehaviour)obj).enabled = value;
        }
        else if (obj is Behaviour)
        {
            ((Behaviour)obj).enabled = value;
        }
        else if (obj is Renderer)
        {
            ((Renderer)obj).enabled = value;
        }
        else if (obj is Collider)
        {
            ((Collider)obj).enabled = value;
        }
        else if (obj is GameObject)
        {
            ((GameObject)obj).SetActive(value);
        }
        else
        {
            print(obj.GetType());
        }
    }

    private bool GetState(Object obj)
    {
        if (obj is MonoBehaviour)
        {
            return ((MonoBehaviour)obj).enabled;
        }
        else if (obj is Behaviour)
        {
            return ((Behaviour)obj).enabled;
        }
        else if (obj is Renderer)
        {
           return  ((Renderer)obj).enabled;
        }
        else if (obj is Collider)
        {
            return ((Collider)obj).enabled;
        }
        else if (obj is GameObject)
        {
            return ((GameObject)obj).activeSelf;
        }
        else
        {
            print(obj.GetType());
            return true;
        }
    }
}

[System.Serializable]
public struct ActivateTriggerTargetData
{
    public Object target;
    public Action onEnterAction;
    public Action onExitAction;
    public float delay;
}

