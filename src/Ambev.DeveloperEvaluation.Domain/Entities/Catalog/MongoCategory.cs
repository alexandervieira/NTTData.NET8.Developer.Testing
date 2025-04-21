using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation.Catalog;

namespace Ambev.DeveloperEvaluation.Domain.Entities.Catalog
{
    public class MongoCategory : BaseEntity
    {
        public string Name { get; private set; } = null!;
        public int Code { get; private set; }

        public MongoCategory() { }

        public MongoCategory(string name, int code)
        {
            Name = name;
            Code = code;            
        }

        public override string ToString()
        {
            return $"{Name} - {Code}";
        }
       
    }
}
