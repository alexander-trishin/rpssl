using System;
using System.Collections.Generic;

using MediatR;

using RPSSL.Application.Game;

namespace RPSSL.Application.Queries.GetChoices
{
    public sealed class GetChoicesQueryHandler : RequestHandler<GetChoicesQuery, IEnumerable<Choice>>
    {
        private readonly IRuleBook _ruleBook;

        public GetChoicesQueryHandler(IRuleBook ruleBook)
        {
            _ruleBook = ruleBook ?? throw new ArgumentNullException(nameof(ruleBook));
        }

        protected sealed override IEnumerable<Choice> Handle(GetChoicesQuery request)
        {
            return _ruleBook.Choices;
        }
    }
}
