using System;

namespace Lab07
{
    class Client
    {
        Server server;
        public event EventHandler<procEventArgs> request;

        public Client(Server server)
        {
            this.server = server;
            request += server.proc;
        }

        public void Send(int num)
        {
            procEventArgs args = new procEventArgs();
            args.id = num;
            OnProc(args);
        }

        protected virtual void OnProc(procEventArgs e)
        {
            EventHandler<procEventArgs> handler = request;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    public class procEventArgs : EventArgs
    {
        public int id { get; set; }
    }
}
