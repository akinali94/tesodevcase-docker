namespace OrderService.Commands;

public interface ICommandHandler<TCommand, TResult>
{
    Task<TResult> Handle(TCommand command);
}