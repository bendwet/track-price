from retriever_factories.price_retriever_factory import save_price


def handler(event, context):
    print('Starting price retriever')
    try:
        save_price()
    except Exception as e:
        print(f'Retrieving price failed with error: {e}')

    print('Ending price retriever')


