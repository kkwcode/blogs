namespace WeakEventSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var tg = new Observable();
            var ob = new Observer();

            WeakEventManager<Observable, EventArgs>.AddHandler(tg, "SomeEvent", ob.OnSome);

            SomeEventWeakEventManager.AddHandler(tg,ob.OnSome);
           
            //ob为弱引用，可以被垃圾回收掉
            ob = null;
            GC.Collect();
            tg.RaiseSomeEvent();

            Console.Read();
        }

        private static void Tg_SomeEvent(EventArgs obj)
        {
            throw new NotImplementedException();
        }
    }

    class Observable
    {
        public event EventHandler<EventArgs> SomeEvent;

        public void RaiseSomeEvent()
        {
            var action = SomeEvent;
            action?.Invoke(this, null);
        }
    }

    class Observer
    {
        public void OnSome(object obj, EventArgs e)
        {
            Console.WriteLine("OnSome Executed!");
        }
    }

    class SomeEventWeakEventManager : WeakEventManager
    {

        private SomeEventWeakEventManager()
        {

        }

        /// <summary>
        /// Add a handler for the given source's event.
        /// </summary>
        public static void AddHandler(Observable source,
                                      EventHandler<EventArgs> handler)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (handler == null)
                throw new ArgumentNullException("handler");

            CurrentManager.ProtectedAddHandler(source, handler);
        }

        /// <summary>
        /// Remove a handler for the given source's event.
        /// </summary>
        public static void RemoveHandler(EventSource source,
                                         EventHandler<EventArgs> handler)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (handler == null)
                throw new ArgumentNullException("handler");

            CurrentManager.ProtectedRemoveHandler(source, handler);
        }

        /// <summary>
        /// Get the event manager for the current thread.
        /// </summary>
        private static SomeEventWeakEventManager CurrentManager
        {
            get
            {
                Type managerType = typeof(SomeEventWeakEventManager);
                SomeEventWeakEventManager manager =
                    (SomeEventWeakEventManager)GetCurrentManager(managerType);

                // at first use, create and register a new manager
                if (manager == null)
                {
                    manager = new SomeEventWeakEventManager();
                    SetCurrentManager(managerType, manager);
                }

                return manager;
            }
        }



        /// <summary>
        /// Return a new list to hold listeners to the event.
        /// </summary>
        protected override ListenerList NewListenerList()
        {
            return new ListenerList<EventArgs>();
        }


        /// <summary>
        /// Listen to the given source for the event.
        /// </summary>
        protected override void StartListening(object source)
        {
            Observable typedSource = (Observable)source;
            typedSource.SomeEvent += new EventHandler<EventArgs>(OnSomeEvent);
        }

        /// <summary>
        /// Stop listening to the given source for the event.
        /// </summary>
        protected override void StopListening(object source)
        {
            Observable typedSource = (Observable)source;
            typedSource.SomeEvent -= new EventHandler<EventArgs>(OnSomeEvent);
        }

        /// <summary>
        /// Event handler for the SomeEvent event.
        /// </summary>
        void OnSomeEvent(object sender, EventArgs e)
        {
            DeliverEvent(sender, e);
        }
    }
}
