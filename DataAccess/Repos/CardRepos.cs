using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using DataAccess.InterfaceRepos;

namespace DataAccess.Repos
{
    public class CardRepos : ICardRepos<Card>
    {
        public Func<IDbConnection> Connection { get; set; }

        public Card Create(Card entity)
        {
            throw new NotImplementedException();
        }

        public Card Delete(Card entity)
        {
            throw new NotImplementedException();
        }

        public Card Get(Card entity)
        {
            var card = new Card();
            const string selectCardSql = "select c.id, c.expires, l.id, l.isactive, co.barcode, co.isAvailable" +
                " from card c left join loan l on l.card_id = c.ID" +
                " left join LoanCopy lc on lc.loanID = l.ID left join copy co on co.Barcode = lc.copyBarcode" +
                " where c.ID = @cardID";
            using (var connection = Connection())
            {
                Loan loan = null;
                connection.Query<Card, Loan, Copy, Card>(selectCardSql,
                    (Card, Loan, Copy) =>
                    {
                        Loan l = null;
                        if(card.ID != Card.ID)
                        {
                            card = new Card {ID = Card.ID, Expires = Card.Expires };
                        }

                        if(Loan != null)
                        {
                            if (!card.Loans.Any(x => x.ID == Loan.ID))
                            {
                                l = Loan;
                                loan = l;
                                card.Loans.Add(loan);
                            }
                            else
                            {
                                loan = card.Loans.Single(x => x.ID == Loan.ID);
                            }

                            if (!loan.Copies.Any(x => x.Barcode == Copy.Barcode) && loan.ID == Loan.ID)
                            {
                                loan.Copies.Add(new Copy { Barcode = Copy.Barcode, IsAvailable = Copy.IsAvailable });
                            }
                        }

                        return card;
                    },
                    new {cardID = entity.ID},
                    splitOn: "ID, ID, Barcode");
            }
            return card;
        }

        public Card Update(Card entity)
        {
            throw new NotImplementedException();
        }
    }
}
