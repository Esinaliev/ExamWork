using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Reflection;

namespace ExamWork
{
    internal class Program
    {
        public const string CppFunctionDll = @"D:\GranFiles\IT step\Проекты\на экзамен\exam\ExamWork\x64\Debug\Dll1.dll";
        [DllImport(CppFunctionDll)]
        public static extern double task1(double num1, double num2, char symbol);

        static async void task1()
        {
            string str = "1";
            while (str != "0")
            {
                str = Console.ReadLine();
                if (str != "0")
                {
                    //тут есть синхронизация
                    double result = await Task.Run(() =>
                    {
                        double num1 = double.Parse(str.Split(' ')[0]);
                        double num2 = double.Parse(str.Split(' ')[1]);
                        char symbol = str.Split(' ')[2][0];
                        return task1(num1, num2, symbol);

                    });
                    Console.WriteLine("результат = " + result);
                }
            }
        }

        static async void task2()
        {
            int which = 1;

            while (which != 0)
            {
                Console.WriteLine();
                Console.WriteLine("0. Главное меню");
                Console.WriteLine("1. Открыть Гугл хром");
                Console.WriteLine("2. Просмотреть все процессы внутри ПК");

                which = int.Parse(Console.ReadLine());
                switch(which)
                {
                    case 1:
                        Thread task = new Thread(() =>
                        {
                            var process = Process.GetCurrentProcess();
                            Process.Start(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", "https://github.com/");
                        });
                        task.Start();
                        break;
                    case 2:
                        //тут есть синхронизация
                        await Task.Run(() =>
                        {
                            var process = Process.GetCurrentProcess();
                            foreach (Process process1 in Process.GetProcesses())
                            {
                                Console.WriteLine($" | ID: {process1.Id} | Name: {process1.ProcessName} | Virtual memory size:{process1.VirtualMemorySize64} |");
                            }
                            return true;
                        });
                        break;
                }
            }
        }

        static async Task task3Async()
        {
            await Task.Run(() =>
            {
                Assembly assembly =  Assembly.LoadFile(Assembly.GetExecutingAssembly().Location);
                List<Module> modules = assembly.Modules.ToList();
                modules.ForEach(n =>
                {
                    Console.WriteLine($"{n.Name}");
                });
            });
        }

        static void task4()
        {
            List<string> keysName = new List<string>();
            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            RegistryKey key;
            int which = 1;
            string name;
            while (which != 0)
            {
                Console.WriteLine();
                Console.WriteLine("1. Создать новый вложенный ключ");
                Console.WriteLine("2. Создать вложенный ключ для существущего ключа");
                Console.WriteLine("3. Создать значении для ключа");
                Console.WriteLine("4. Удалить");
                Console.WriteLine("показать значения (5)");
                which = int.Parse(Console.ReadLine());

                switch (which)
                {
                    case 0:
                        Console.WriteLine("До свидания!");
                        break;
                    case 1:
                        name = Console.ReadLine();
                        Task.Run(() =>
                        {
                            key = Registry.CurrentUser;
                            key.CreateSubKey(name);
                            key.Close();
                            keysName.Add(name);
                        });
                        break;
                    case 2:
                        name = Console.ReadLine();
                        string newName = Console.ReadLine();
                        Task.Run(() =>
                        {
                            key = (RegistryKey)Registry.CurrentUser.OpenSubKey(name, true);
                            key.CreateSubKey(newName);
                            key.Close();
                            keysName.Add(newName);
                        });
                        break;
                    case 3:
                        name = Console.ReadLine();
                        string paramName = Console.ReadLine();
                        string value = Console.ReadLine();
                        Task.Run(() =>
                        {
                            key = (RegistryKey)Registry.CurrentUser.OpenSubKey(name, true);
                            key.SetValue(paramName, value);
                            key.Close();
                            keyValues.Add(name, paramName);
                        });
                        break;
                    case 4:
                        //name = Console.ReadLine();
                        Task.Run(() =>
                        {
                            keysName.Reverse();
                            keysName.ForEach(n =>
                            {
                                key = (RegistryKey)Registry.CurrentUser.OpenSubKey(n, true);
                                keyValues.ToList().ForEach(v =>
                                {
                                    if (v.Key == n) key.DeleteValue(v.Value);
                                });
                                key.Close();
                            });
                        });
                        break;
                    case 5:
                        name = Console.ReadLine();
                        string value1 = Console.ReadLine();
                        Task.Run(() =>
                        {
                            key = (RegistryKey)Registry.CurrentUser.OpenSubKey(name);
                            Console.WriteLine(key.GetValue(value1));
                            key.Close();
                        });
                        break;

                }
                //Console.ReadKey();
            }

        }

        static void task5()
        {
            Task.Run(() =>
            {
                var f = new Form();
                f.AutoSize = true;
                PictureBox picture = new PictureBox() { ImageLocation = @"D:\GranFiles\IT step\Проекты\на экзамен\exam\ExamWork\ExamWork\image\906324.png", SizeMode = PictureBoxSizeMode.AutoSize, Dock = DockStyle.Fill };
                f.Controls.Add(picture);
                f.Show();
                Console.WriteLine(picture);
            });
        }
        static async void task6Async()
        {
            List<string> cars = new List<string>
                {
                    "Mercedes-Benz",
                    "Audi",
                    "Honda",
                    "BMW",
                    "Toyoto",
                    "Nissan"
                };

            cars.AsParallel().Select(n => n).ForAll(Console.WriteLine);
        }
        static async void task7()
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;
            List<Task> TaskList = new List<Task>();

            for (int i = 0; i < 3; i++)
            {
                Task task = new Task(() => {
                    Console.WriteLine("Hi Itstep");
                }, token);
                TaskList.Add(task);
                task.Start();
            }
            //cancelTokenSource.Cancel();
            Task.WaitAll(TaskList.ToArray());
        }
        static void Main(string[] args)
        {
            int which = 1;
            while (which != 0)
            {
                Console.WriteLine();
                Console.WriteLine("1. DLL Import");
                Console.WriteLine("2. Открыть Процессы");
                Console.WriteLine("3. Список Сборки");
                Console.WriteLine("4. Реестр");
                Console.WriteLine("5. Вывести картинку");
                Console.WriteLine("6. Вывести список Машин");
                Console.WriteLine("7. Вывести слово Hi ItStep");
                which = int.Parse(Console.ReadLine());

                switch(which)
                {
                    case 0:
                        Console.WriteLine("До свидания!");
                        break;
                    case 1:
                        task1();
                        break;
                    case 2:
                        task2();
                        break;
                    case 4:
                        task4();
                        break;
                    case 5:
                        task5();
                        break;
                    case 6:
                        task6Async();
                        break;
                    case 7:
                        task7();
                        break;

                }
                //Console.ReadKey();
            }
        }
    }
}
