SELECT
    p.product_id
     , MIN(p.price_sale) as price_sale
     , p.is_available
     , p.price_date
FROM
    prices p
WHERE
    p.product_id = @productId
    AND
      p.is_available = true
GROUP BY
    p.price_date;