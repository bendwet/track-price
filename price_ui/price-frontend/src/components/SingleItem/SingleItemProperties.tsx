import { useSelector } from "react-redux";
import { RootState } from '../../stores/store';
import ProductModel from '../ProductModel';

export default function SingleItemProperties() {

  const { selectedProduct } = useSelector((state: RootState) => state.singleProduct);
	
  return (
    <div>
      <div className='SingleItemTitle'>
       <h1>{selectedProduct.product_name}</h1>
      </div>
      <div className='SinleItemProperties'>
        Properties
      </div>
    </div>
  )
}
