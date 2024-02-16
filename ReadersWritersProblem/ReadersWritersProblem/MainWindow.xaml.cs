using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ReadersWritersProblem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private readonly object _syncRoot = new object();
        private readonly object _syncWrite = new object();
        private int _rcount = 0;
        private int _wcount = 0;
        public void EnterReadLock()
        {
            lock (_syncRoot)
            {
                while (_wcount > 0)
                {
                    Monitor.Wait(_syncRoot);
                }

                _rcount++;
            }
        }

        public void EnterWriteLock()
        {
            lock (_syncRoot)
            {
                _wcount++;

                while (_rcount > 0)
                {
                    Monitor.Wait(_syncRoot);
                }
            }

            Monitor.Enter(_syncWrite);
        }
        public void ExitReadLock()
        {
            lock (_syncRoot)
            {
                if (--_rcount == 0 && _wcount > 0)
                {
                    Monitor.PulseAll(_syncRoot);
                }
            }
        }
        
        public void ExitWriteLock()
        {
            Monitor.Exit(_syncWrite);
        
            lock (_syncRoot)
            {
                if (--_wcount == 0)
                {
                    Monitor.PulseAll(_syncRoot);
                }
            }
        }
    }
}