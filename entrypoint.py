from retriever_factories.price_retriever_factory import save_price


def handler(event, context):
    print("This is a test message to see if this thing is working")
    save_price()



