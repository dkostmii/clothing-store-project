import './Order.css'

export default function Order({ orderData }) {
  if (!orderData) {
    throw new Error("Missing order data")
  }

  const { orderDate, orderSummary, shippingInfo } = orderData

  return (
    <div className="order">
      <div>
        <label>{orderDate}</label>
        <label>Order Date</label>
      </div>

      <div className="shipping-info">
        <div>
          <label>{shippingInfo.country}</label>
          <label>Country</label>
        </div>
        <div>
          <label>{shippingInfo.city}</label>
          <label>City</label>
        </div>
        <div>
          <label>{shippingInfo.address}</label>
          <label>Address</label>
        </div>
        <div>
          <label>{shippingInfo.postalCode}</label>
          <label>Postal Code</label>
        </div>
        <div>
          <label>{shippingInfo.contactPhone}</label>
          <label>Contact phone</label>
        </div>
      </div>

      <div className="summary">
        <label>{orderSummary}zł</label>
        <label>Order Summary</label>
      </div>
    </div>
  )
}