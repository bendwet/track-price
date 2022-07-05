WITH
    last_date_per_product as (
        SELECT
            prices.product_id
             , MAX(price_date) as max_date
        FROM
            prices
        GROUP BY
            prices.product_id
    )
   , product_out_of_stock as (
    SELECT
        p.product_id
    FROM
        prices p
            INNER JOIN last_date_per_product ldpp on ldpp.product_id = p.product_id
            AND p.price_date = ldpp.max_date
    GROUP BY
        product_id
    HAVING
            MAX(p.price_sale) = 0
)
   , lowest_price_per_product_day_ranked as (
    SELECT
        p.price_id
         , p.product_id
         , products.product_name
         , p.price_sale
         , p.price_quantity
         , p.is_available
         , p.is_onsale
         , p.price_date
         , RANK() OVER (PARTITION BY p.product_id ORDER BY p.price_sale ASC) as price_rank
    FROM
        prices p
            INNER JOIN last_date_per_product ldpp ON ldpp.product_id = p.product_id
            AND p.price_date = ldpp.max_date
            INNER JOIN products ON p.product_id = products.product_id
    WHERE
            p.is_available = true
)
   , lowest_price_per_product_day as (
    SELECT
        *
    FROM
        lowest_price_per_product_day_ranked
    WHERE
            price_rank = 1
    GROUP BY
        product_id
)
SELECT
    p.product_id
     , p.product_name
     , lpd.price_sale
     , lpd.price_quantity
     , lpd.is_available
     , lpd.is_onsale
     , lpd.price_date
     , case when pos.product_id is null then 0 else 1 end as product_is_out_of_stock
FROM
    products p
        LEFT JOIN lowest_price_per_product_day lpd ON lpd.product_id = p.product_id
        LEFT JOIN product_out_of_stock pos ON pos.product_id = p.product_id