WITH
    last_date_for_product_per_store as (
        SELECT
            p.product_id
             , p.store_id
             , MAX(p.price_date) as max_date
        FROM
            prices p
        WHERE
            p.product_id = @productId
        GROUP BY
            p.store_id
    )
   , product_price_per_store as (
        SELECT
            ldps.product_id
             , ldps.store_id
             , pr.product_name
             , s.store_name
             , p.price_sale
             , p.is_available
             , p.is_onsale
             , p.price_quantity
             , ldps.max_date as price_date
        FROM
            prices p
                INNER JOIN last_date_for_product_per_store ldps ON ldps.product_id = p.product_id
                    AND ldps.max_date = p.price_date AND ldps.store_id = p.store_id
                INNER JOIN products pr ON pr.product_id = p.product_id
                INNER JOIN stores s ON s.store_id = p.store_id

    )
SELECT *
FROM
    product_price_per_store