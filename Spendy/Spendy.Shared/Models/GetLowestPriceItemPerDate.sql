SELECT
    p.product_id
     , p.price_sale
     , p.is_available
     , MIN(p.price_date) as price_date
FROM
    prices p
WHERE
    p.product_id = @productId
GROUP BY
    p.price_date;