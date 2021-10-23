import { useState, useEffect } from 'react'

import Order from '../../Components/Order'

import { OrdersRequest as Request } from '../../Services/Request'

import './OrdersPage.css'

function OrdersPage({ host, authentication }) {
  if (!host) {
    throw new Error("Host is not provided")
  }

  if (!authentication) {
    throw new error("authentication data is not provided")
  }

  const { user, token } = authentication

  const [json, setJson] = useState()
  const [error, setError] = useState()
  const [loaded, setLoaded] = useState(false)


  useEffect(() => {
    if (!loaded && !error) {
      Request(json => {
        setJson(json)
        setLoaded(true)
      }, error => {
        console.error(error)
        setError(error)
      }, host, { id: user.id, token })
    }
  }, [error, loaded, user.id, token, host])

  const errorMessage = <p style={{color: '#F00'}}>Error loading orders: {error}</p>
  const loadingMessage = <p>Loading orders...</p>

  const orders = (json && Array.isArray(json)) && json.map((order, id) => {
    return <Order key={id} orderData={order} />
  })

  return (
    <div className="orders-page-container">
      { loaded ? 
      orders
        :
      loadingMessage }
      { error && errorMessage }
    </div>
  )
}

export default OrdersPage