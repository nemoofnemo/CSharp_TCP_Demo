using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    class CustomEvent
    {
        private string msg;
        public string Msg
        {
            get { return msg; }
            set { msg = value; }
        }
    }

    class Publisher
    {
        public event EventHandler<CustomEvent> OnMessageEvent;

        public void raiseEvent()
        {
            //....
            OnRaiseEvent(new CustomEvent());
        }

        protected void OnRaiseEvent(CustomEvent e)
        {
            EventHandler<CustomEvent> handler = OnMessageEvent;
            if(handler != null)
            {
                handler(this, e);
            }
        }
    }

    class Subscriber
    {
        private string ID;

        public Subscriber(string id, Publisher pub)
        {
            ID = id;
            pub.OnMessageEvent += OnEventRaise;
        }

        private void OnEventRaise(object sender, CustomEvent e)
        {
            Console.Write("in {0}, OnEventRaise invoked", ID);
        }

        public int this[int i]
        {
            get { return i * i; }
        }

        public virtual void show()
        {
            Console.WriteLine("base show");
        }
    }

    class AC : Subscriber
    {
        public AC(string id, Publisher pub) : base(id, pub)
        {

        }

        public override void show()
        {
            Console.WriteLine("derived show");
        }
    }

    class Start
    {
        public static void Main()
        {
            byte[] arr = new byte[5];
            arr[0] = (byte)'a';
            arr[1] = (byte)'b';

            Console.WriteLine("{0}", arr.ToString());

        }
    }
}