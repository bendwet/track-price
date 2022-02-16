import { useParams } from "react-router-dom";


export default function SingleItemListData() {
	let {productID} = useParams();
  return (
    <div>
			Item{productID}
    </div>
  );
}
