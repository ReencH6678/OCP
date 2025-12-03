using System;
using System.Collections.Generic;

namespace IMJunior
{
    class Program
    {
        static void Main(string[] args)
        {
            BankSystemFactoryProvider bankSystemFabricProvider = new BankSystemFactoryProvider();
            OrderForm orderForm = new OrderForm();
            
            IBankSystemFabric bankSystemFabric = bankSystemFabricProvider.GetFabric(orderForm.GetBankId(bankSystemFabricProvider.SystemIDs));
            PaymentHandler paymentHandler = new PaymentHandler(bankSystemFabric);

            paymentHandler.Pay();
        }
    }

    public class OrderForm
    {
        public string GetBankId(IEnumerable<string> bankSystemFabricIDs)
        {
            Console.WriteLine("Мы принимаем: ");

            foreach (string id in bankSystemFabricIDs)
                Console.Write($"{id}, ");

            Console.WriteLine("Какое системой вы хотите совершить оплату?");

            return Console.ReadLine();
        }
    }

    public class BankSystemFactoryProvider
    {
        public Dictionary<string, IBankSystemFabric> _fabrics = new Dictionary<string, IBankSystemFabric>()
        {
            {"Qiwi", new QiwiFabric() },
            {"WebMoney", new WebMoneyFabric() },
            {"Card", new CardFabric() }
        };

        public IEnumerable<string> SystemIDs => _fabrics.Keys;

        public IBankSystemFabric GetFabric(string systemId)
        {
            if(string.IsNullOrWhiteSpace(systemId)) 
                throw new ArgumentNullException();

            if (_fabrics.TryGetValue(systemId, out IBankSystemFabric bankSystemFabric) == false)
                throw new ArgumentException();

            return bankSystemFabric;
        }
    }

    public class QiwiFabric : IBankSystemFabric 
    {
        public IBankSystem Create()
        {
            return new Qiwi();
        }
    }

    public class WebMoneyFabric : IBankSystemFabric 
    {
        public IBankSystem Create()
        {
            return new WebMoney();
        }
    }

    public class CardFabric : IBankSystemFabric 
    {
        public IBankSystem Create()
        {
            return new Card();
        }
    }

    public class PaymentHandler
    {
        private readonly IBankSystem _bankSystem;

        public PaymentHandler(IBankSystemFabric bankSystemFabric) 
        {
            if (bankSystemFabric == null)
                throw new ArgumentNullException();

            _bankSystem = bankSystemFabric.Create();
        }

        public void Pay()
        {
            _bankSystem.CallApi();
            _bankSystem.TryPay();
            _bankSystem.ShowInfo();

            Console.WriteLine("Оплата прошла успешно!");
        }
    }

    public class Qiwi : IBankSystem
    {
        public void CallApi()
        {
            Console.WriteLine($"Перевод на страницу Qiwi...");
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Вы оплатили с помощью Qiwi");
        }

        public void TryPay()
        {
            Console.WriteLine($"Проверка платежа через Qiwi...");
        }
    }

    public class WebMoney : IBankSystem
    {
        public void CallApi()
        {
            Console.WriteLine("Вызов API WebMoney...");
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Вы оплатили с помощью WebMoney");
        }

        public void TryPay()
        {
            Console.WriteLine("Проверка платежа через WebMoney...");
        }
    }

    public class Card : IBankSystem
    {
        public void CallApi()
        {
            Console.WriteLine("Вызов API банка эмитера карты Card...");
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Вы оплатили с помощью Card");
            
        }

        public void TryPay()
        {
            Console.WriteLine("Проверка платежа через Card...");
        }
    }

    public interface IBankSystem
    {
        void ShowInfo();
        void CallApi();
        void TryPay();

    }

    public interface IBankSystemFabric
    {
        IBankSystem Create();
    }

    public enum Banks
    {
        Qiwi,
        WebMoney,
        Card
    }
}