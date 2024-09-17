using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using DG.Tweening;

public class Bus : MonoBehaviour
{
    public int AllCapacity;
    public int BusSeats;
    public Transform passengerParent; // Yolcularýn minibüste duracaðý yer
    public float pickupRange = 5f; // Yolcu alma mesafesi
    public List<Passenger> passengers;
    public List<Passenger> passengerStanding;
    public List<SitPoint> sitPoints;
    public List<Transform> ExitDoorPoint;
    public StopArea CurrentStopAreaBus;
    public bool IsPassengerCrouched;

    public RectTransform PopPolicePenalty;

    private void Update()
    {
        // Yolcu alma tuþuna basýldýðýnda (örneðin "E" tuþu)
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickupPassenger();
        }

        // Yolcu býrakma tuþuna basýldýðýnda (örneðin "Q" tuþu)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropOffPassengers();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            CrouchingPassenger();
        }
    }

    

    private void CrouchingPassenger()
    {
       if (passengerStanding.Count>0) 
       {
            IsPassengerCrouched = !IsPassengerCrouched;
            foreach (var passenger in passengerStanding)
            {
                passenger.OnCrouching(IsPassengerCrouched);
            }

       }
       else
        IsPassengerCrouched = false;


    }

    private void TryPickupPassenger()
    {
        // Oyundaki tüm yolcularý tarýyoruz
        Passenger[] allPassengers = FindObjectsOfType<Passenger>();
        foreach (Passenger passenger in allPassengers)
        {
            if (!passenger.isPickedUp && passengers.Count < sitPoints.Count)
            {
                // Yolcu minibüse yeterince yakýn mý?
                if (Vector3.Distance(transform.position, passenger.transform.position) <= pickupRange)
                {
                    // Yolcuyu minibüse ekliyoruz
                    passengers.Add(passenger);

            

                    foreach (SitPoint point in sitPoints)
                    {
                        if (!point.IsPicked && !point.IsStanded)
                        {
                            passenger.OnPickedUp(point, false);
                            point.IsPicked = true;
                            break;
                        }
                        else if (!point.IsPicked && point.IsStanded)
                        {
                            passenger.OnPickedUp(point, true);
                            point.IsPicked = true;
                            passengerStanding.Add(passenger);

                            break;

                        }
                    }
                    break; // Bir yolcu alýndýktan sonra döngüyü sonlandýr
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<AreaTrigger>(out AreaTrigger AreaTrigger))
        {
            CurrentStopAreaBus = AreaTrigger.Area;

            if(AreaTrigger.Area == StopArea.PolicePenaltyArea && !IsPassengerCrouched && passengerStanding.Count > 0) 
            {
                PolicePenalty();
            }
        }

    }

    private void PolicePenalty()
    {
        PopPolicePenalty.gameObject.SetActive(true);
        PopPolicePenalty.GetComponent<CanvasGroup>().alpha = 1;
        PopPolicePenalty.DOScale(Vector3.one, 1).From(Vector3.zero);

        PopPolicePenalty.GetComponent<CanvasGroup>().DOFade(0, 4);

        DOVirtual.DelayedCall(4f, () =>
        {
            PopPolicePenalty.gameObject.SetActive(false);
        });

    }


    private void DropOffPassengers()
    {

        List<Passenger>  PassengersAtSameTargetStop =  new List<Passenger>();
        List<Passenger> PassengersAtSameTargetStopSingle = new List<Passenger>();
        // Tüm yolcularý minibüsten indiriyoruz
        foreach (Passenger passenger in passengers)
        {
            foreach(Passenger _passenger in passengers)
            {
                if (passenger != _passenger && passenger.TargetStopArea == _passenger.TargetStopArea)
                {
                    if (!PassengersAtSameTargetStop.Contains(passenger))
                    {
                        if (passenger.TargetStopArea == CurrentStopAreaBus)
                        {
                            PassengersAtSameTargetStop.Add(passenger);
                        }
                    }
                }
            }
            if (PassengersAtSameTargetStop.Count ==0)
            {
                if(CurrentStopAreaBus == passenger.TargetStopArea)
                {
                    PassengersAtSameTargetStopSingle.Add(passenger);

                }
            }

        }
        if (PassengersAtSameTargetStop.Count ==0 && PassengersAtSameTargetStopSingle.Count > 0)
        {
            { PassengersAtSameTargetStopSingle[0].OnDroppedOff(ExitDoorPoint[0]);
           
              passengers.Remove(PassengersAtSameTargetStopSingle[0]);

             if(passengerStanding.Contains(PassengersAtSameTargetStopSingle[0]))
              passengerStanding.Remove(PassengersAtSameTargetStopSingle[0]);
                
              
                

            }
        }
        else
        {
            for (int i = 0; i < PassengersAtSameTargetStop.Count; i++)
            {

                Transform dropOffPoint = (i < ExitDoorPoint.Count) ? ExitDoorPoint[i] : ExitDoorPoint[0];
                PassengersAtSameTargetStop[i].OnDroppedOff(dropOffPoint);
                passengers.Remove(PassengersAtSameTargetStop[i]);

                if (passengerStanding.Contains(PassengersAtSameTargetStop[i]))
                passengerStanding.Remove(PassengersAtSameTargetStop[i]);
                
            }
        }

       

    }
}
