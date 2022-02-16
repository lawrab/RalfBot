namespace SnailRacing.Ralf.Infrastrtucture
{
    public abstract class ValidatorBase<TModel> : IValidate<TModel>
    {
        private List<Func<TModel, string>> _rules = new List<Func<TModel, string>>();

        public IEnumerable<string> IsValid(TModel model)
        {
            return _rules.Select(r => r(model))
                .Where(r => !string.IsNullOrEmpty(r));
        }

        public void AddRule(Func<TModel, string> rule)
        {
            _rules.Add(rule);
        }
    }
}
