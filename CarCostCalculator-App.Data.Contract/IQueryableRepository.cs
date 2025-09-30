﻿namespace CarCostCalculator_App.Data.Contract
{
    public interface IQueryableRepository<out TContract>
    where TContract : class
    {
        #region Public Methods

        IQueryable<TContract> QueryProjection();

        #endregion
    }
}
