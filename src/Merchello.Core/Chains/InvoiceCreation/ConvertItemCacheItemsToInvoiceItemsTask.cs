﻿using System;
using Merchello.Core.Models;
using Merchello.Core.Sales;
using Umbraco.Core;

namespace Merchello.Core.Chains.InvoiceCreation
{
    /// <summary>
    /// Converts ItemCacheLineItem(s) to InvoiceLineItems
    /// </summary>
    internal class ConvertItemCacheItemsToInvoiceItemsTask : InvoiceCreationAttemptChainTaskBase
    {
        public ConvertItemCacheItemsToInvoiceItemsTask(SalePreparationBase salePreparation) 
            : base(salePreparation)
        {}

        /// <summary>
        /// Task converts ItemCacheLineItems to InvoiceLineItems and adds them to the invoice
        /// </summary>
        /// <param name="value">The <see cref="IInvoice"/> to which to add the line items</param>
        /// <returns>The <see cref="Attempt"/></returns>
        public override Attempt<IInvoice> PerformTask(IInvoice value)
        {
            foreach (var lineItem in SalePreparation.ItemCache.Items)
            {
                try
                {                       
                    value.Items.Add(lineItem.AsLineItemOf<InvoiceLineItem>());
                }
                catch (Exception ex)
                {
                    return Attempt<IInvoice>.Fail(ex);
                }                
            }

            return Attempt<IInvoice>.Succeed(value);
        }
    }
}