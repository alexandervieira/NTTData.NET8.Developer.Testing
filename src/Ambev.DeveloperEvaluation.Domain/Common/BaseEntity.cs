using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvoluation.Core.Messages;

namespace Ambev.DeveloperEvaluation.Domain.Common;

public abstract class BaseEntity : IComparable<BaseEntity>
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    private List<Event> _notifications = new List<Event>();
    public IReadOnlyCollection<Event> Notifications => _notifications.AsReadOnly();

    public BaseEntity()
    {
        Id = Guid.NewGuid();
    }

    public Task<IEnumerable<ValidationErrorDetail>> ValidateAsync()
    {
        return Validator.ValidateAsync(this);
    }

    public void AddEvent(Event @event)
    {
        _notifications ??= new List<Event>();
        _notifications.Add(@event);
    }

    public void RemoveEvent(Event eventItem)
    {
        _notifications?.Remove(eventItem);
    }

    public void ClearEvents()
    {
        _notifications?.Clear();
    }

    public int CompareTo(BaseEntity? other)
    {
        if (other == null)
        {
            return 1;
        }

        return other!.Id.CompareTo(Id);
    }
    public override string ToString()
    {
        return $"{GetType().Name} [Id={Id}]";
    }

    public virtual bool IsValid()
    {
        throw new NotImplementedException();
    }
}
