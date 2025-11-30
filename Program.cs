using System;

namespace IMJunior
{
    class Program
    {
        static void Main(string[] args)
        {
            BankSystemFabric bankSystemFabric = new BankSystemFabric();
            OrderForm orderForm = new OrderForm(bankSystemFabric);
            PaymentHandler paymentHandler = new PaymentHandler();

            IBankSystem systemId = orderForm.ShowForm();

            systemId.CallApi();
            systemId.TryPay();
            paymentHandler.ShowPaymentResult(systemId);
        }
    }

    public class OrderForm
    {
        private readonly BankSystemFabric _bankSystemFabric;

        public OrderForm(BankSystemFabric bankSystemFabric)
        {
            if (bankSystemFabric == null)
                throw new ArgumentNullException();

            _bankSystemFabric = bankSystemFabric;
        }

        public IBankSystem ShowForm()
        {
            Console.WriteLine($"Мы принимаем: {Banks.Qiwi}, {Banks.WebMoney}, {Banks.Card}");

            Console.WriteLine("Какое системой вы хотите совершить оплату?");

            IBankSystem chosenSystem = _bankSystemFabric.Create(Console.ReadLine());

            if (chosenSystem == null)
                throw new ArgumentNullException();

            return chosenSystem;
        }
    }

    public class BankSystemFabric
    {
        public IBankSystem Create(string name)
        {
            switch(name)
            {
                case nameof(Banks.Qiwi):
                    return new Qiwi("Quwi");
                case nameof(Banks.WebMoney):
                    return new WebMoney("WebMoney");
                case nameof(Banks.Card):
                    return new Card("Card");
                default:
                    return null;
            }
        }
    }

    public class PaymentHandler
    {
        public void ShowPaymentResult(IBankSystem systemId)
        {
            systemId.ShowInfo();
            Console.WriteLine("Оплата прошла успешно!");
        }
    }

    public class BankSystem
    {
        public BankSystem(string name)
        {
            if(name == null) 
                throw new ArgumentNullException();

            Name = name;
        }
    
        public string Name { get; private set; }
    }

    public class Qiwi : BankSystem, IBankSystem
    {
        public Qiwi(string name) : base(name)
        {
        }

        public void CallApi()
        {
            Console.WriteLine("Перевод на страницу QIWI...");
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Вы оплатили с помощью {Name}");
        }

        public void TryPay()
        {
            Console.WriteLine("Проверка платежа через QIWI...");
        }
    }

    public class WebMoney : BankSystem, IBankSystem
    {
        public WebMoney(string name) : base(name)
        {
        }

        public void CallApi()
        {
            Console.WriteLine("Вызов API WebMoney...");
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Вы оплатили с помощью {Name}");
        }

        public void TryPay()
        {
            Console.WriteLine("Проверка платежа через WebMoney...");
        }
    }

    public class Card : BankSystem, IBankSystem
    {
        public Card(string name) : base(name)
        {
        }

        public void CallApi()
        {
            Console.WriteLine("Вызов API банка эмитера карты Card...");
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Вы оплатили с помощью {Name}");
            
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

    public enum Banks
    {
        Qiwi,
        WebMoney,
        Card
    }
}