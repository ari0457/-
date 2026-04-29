using System;

interface ICommand
{
    void Execute();
}

class BankAccountService
{
    public void Transfer(decimal amount, string fromAccount, string toAccount)
    {
        Console.WriteLine($"Перевод {amount} руб. со счета {fromAccount} на счет {toAccount} выполнен");
    }
}

class TransferMoneyCommand : ICommand
{
    private BankAccountService receiver;
    private decimal amount;
    private string fromAccount;
    private string toAccount;

    public TransferMoneyCommand(BankAccountService receiver, decimal amount, string fromAccount, string toAccount)
    {
        this.receiver = receiver;
        this.amount = amount;
        this.fromAccount = fromAccount;
        this.toAccount = toAccount;
    }

    public void Execute()
    {
        receiver.Transfer(amount, fromAccount, toAccount);
    }
}

class BankingTerminal
{
    private ICommand command;

    public void SetCommand(ICommand command)
    {
        this.command = command;
    }

    public void ExecuteCommand()
    {
        command.Execute();
    }
}

class Program
{
    static void Main()
    {
        BankAccountService service = new BankAccountService();

        TransferMoneyCommand transfer1 = new TransferMoneyCommand(service, 500, "123456", "654321");
        TransferMoneyCommand transfer2 = new TransferMoneyCommand(service, 1200, "111111", "222222");
        TransferMoneyCommand transfer3 = new TransferMoneyCommand(service, 75, "333333", "444444");

        BankingTerminal terminal = new BankingTerminal();

        terminal.SetCommand(transfer1);
        terminal.ExecuteCommand();

        terminal.SetCommand(transfer2);
        terminal.ExecuteCommand();

        terminal.SetCommand(transfer3);
        terminal.ExecuteCommand();
    }
}