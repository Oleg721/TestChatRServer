using DTO.Contracts;
using System;


namespace DTO.Contracts
{
    public abstract class BaseEntity<TId>
    {
        public TId Id { get; set; }  
    }
}
