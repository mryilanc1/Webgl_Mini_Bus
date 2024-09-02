using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class Bus : MonoBehaviour
{
    public Transform passengerParent; // Yolcularýn minibüste duracaðý yer
    public float pickupRange = 5f; // Yolcu alma mesafesi
    public List<Passenger> passengers;
    public List<SitPoint> sitPoints;
    public List<Transform> ExitDoorPoint;
    public StopArea CurrentStopAreaBus;

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
    }

    private void TryPickupPassenger()
    {
        // Oyundaki tüm yolcularý tarýyoruz
        Passenger[] allPassengers = FindObjectsOfType<Passenger>();
        foreach (Passenger passenger in allPassengers)
        {
            if ((!passenger.isPickedUp) && (passengers.Count < sitPoints.Count))
            {
                // Yolcu minibüse yeterince yakýn mý?
                if (Vector3.Distance(transform.position, passenger.transform.position) <= pickupRange)
                {
                    // Yolcuyu minibüse ekliyoruz
                    passengers.Add(passenger);

                    foreach (SitPoint point in sitPoints)
                    {
                        if(!point.IsPicked)
                        {
                            passenger.OnPickedUp(point);
                            point.IsPicked = true;
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

        }
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
                

            }
        }
        else
        {
            for (int i = 0; i < PassengersAtSameTargetStop.Count; i++)
            {

                Transform dropOffPoint = (i < ExitDoorPoint.Count) ? ExitDoorPoint[i] : ExitDoorPoint[0];
                PassengersAtSameTargetStop[i].OnDroppedOff(dropOffPoint);
                passengers.Remove(PassengersAtSameTargetStop[i]);
            }
        }

       

    }
}
