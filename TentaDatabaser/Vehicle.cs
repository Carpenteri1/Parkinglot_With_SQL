using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TentaDatabaser
{
    class Vehicle
    {

       public string Regnumber { get; set; }
       public DateTime TimeArrived { get; set;}
       public VehicleType Type  { get; set; }
       public int SlotPosition { get; set; }
       public int Size { get; set; }
       public TimeSpan TimeParked { get { return Calculate.TimeParked(this); } set{;}}
       public decimal Cost { get { return Calculate.Price(this.Type, this); } set{;}}

       public string ToString()
       {
           return $"Regnumber: {Regnumber}  \nVehicleType: {Type} \nSlotPosition: {SlotPosition} \nTimeArrived: {TimeArrived} \nTimeParked: {TimeParked} \nCost: {Cost}";
       }

    }
}
