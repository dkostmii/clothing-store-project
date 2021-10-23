import { Role } from '../Types/Role'

export function DeleteProduct(onSuccess, onFail, host, requestData) {
  if (!requestData) {
    throw new Error("requestData is not provided")
  }

  const { id, token } = requestData

  if (!id) {
    throw new Error("requestData.id is not provided")
  }

  if (!token) {
    throw new Error("requestData.token is not provided")
  }

  fetch(`${host}/Products/${id}`, {
    method: 'DELETE',
    headers: {
      'Content-Type': 'application/json',
      'withCredentials': 'true',
      'Authorization': 'Bearer ' + token
    }
  })
  .then(response => {
    if (!response.ok) {
      throw new Error("Failed removing product")
    }

    onSuccess()
  })
  .catch(error => {
    onFail(error.toString())
  })
}

function AddProductImage(onSuccess, onFail, host, requestData) {
  if (!requestData) {
    throw new Error("requestData is not provided")
  }

  const { id, token, productId, image } = requestData

  if (!id) {
    throw new Error("requestData.id is not provided")
  }

  if (!token) {
    throw new Error("requestData.token is not provided")
  }

  if (!productId) {
    throw new Error("requestData.productId is not provided")
  }

  if (!image) {
    throw new Error("requestData.data is not provided")
  }

  fetch(`${host}/Products/Images?productId=${productId}`, {
    method: 'POST',
    body: image
  })
  .then(response => {
    if (!response.ok) {
      throw new Error("Error uploading image")
    }
    return response.json()
  })
  .then(json => {
    onSuccess(json)
  })
  .catch(error => {
    onFail(error.toString())
  })
}

function AddProduct(onSuccess, onFail, host, requestData) {
  if (!requestData) {
    throw new Error("requestData is not provided")
  }

  const { id, token, data } = requestData

  if (!id) {
    throw new Error("requestData.id is not provided")
  }

  if (!token) {
    throw new Error("requestData.token is not provided")
  }

  if (!data) {
    throw new Error("requestData.data is not provided")
  }

  fetch(`${host}/Products`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'withCredentials': 'true',
      'Authorization': 'Bearer ' + token
    },
    body: JSON.stringify(data)
  })
  .then(response => {
    if (!response.ok) {
      throw new Error("Failed adding new product")
    }

    return response.json()
  })
  .then(json => {
    onSuccess(json)
  })
  .catch(error => {
    onFail(error.toString())
  })
}

export function AddProductRequest(onSuccess, onFail, host, requestData) {
  if (!requestData) {
    throw new Error("requestData is not provided")
  }

  if (!requestData.image) {
    throw new Error("RequestData.image is not provided")
  }

  if (!requestData.data) {
    throw new Error("RequestData.data is not provided")
  }

  AddProduct(first => {
    AddProductImage(second => {
      onSuccess(second.message)
    }, error => {
      onFail(error)
    }, host, { ...requestData, productId: first.product.id })
  }, error => {
    onFail(error)
  }, host, requestData)
}

export function BaseDataRequest(onSuccess, onFail, host) {
  const materials = fetch(`${host}/Products/Materials`)
  .then(response => {
    if (response.ok) {
      return response.json()
    }

    throw new Error("Failed loading customer balance")
  })
  .then(json => {
    return { materials: json }
  })
  .catch(error => {
    onFail(error.toString())
  })

  const types = fetch(`${host}/Products/Types`)
  .then(response => {
    if (response.ok) {
      return response.json()
    }

    throw new Error("Failed loading customer balance")
  })
  .then(json => {
    return { types: json }
  })
  .catch(error => {
    onFail(error.toString())
  })

  const sizes = fetch(`${host}/Products/Sizes`)
  .then(response => {
    if (response.ok) {
      return response.json()
    }

    throw new Error("Failed loading customer balance")
  })
  .then(json => {
    return { sizes: json }
  })
  .catch(error => {
    onFail(error.toString())
  })

  Promise.all([materials, sizes, types]).then(
    values => {
      return values.reduce((acc, val) => { 
        return { ...acc, ...val } 
      })
    }
  )
  .then(result => onSuccess(result))
  .catch(error => {
    onFail(error.toString())
  })
}

export function CustomerBalanceRequest(onSuccess, onFail, host, requestData) {
  if (!requestData) {
    throw new Error("requestData is not provided")
  }

  const { id, token } = requestData

  if (!id) {
    throw new Error("requestData.id is not provided")
  }

  if (!token) {
    throw new Error("requestData.token is not provided")
  }

  fetch(`${host}/Customer/${id}/Balance`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'withCredentials': 'true',
      'Authorization': 'Bearer ' + token
    }
  })
  .then(response => {
    if (response.ok) {
      return response.json()
    }

    throw new Error("Failed loading customer balance")
  })
  .then(json => {
    onSuccess(json)
  })
  .catch(error => {
    onFail(error.toString())
  })
}

export const CartRequests = {
  Get: GetCartRequest,
  Post: AddToCartRequest,
  Delete: DeleteFromCartRequest,
  DeleteAll: ClearCartRequest
}

function GetCartRequest(onSuccess, onFail, host, requestData) {
  if (!requestData) {
    throw new Error("Request data is not provided")
  }

  const { id, token } = requestData

  if (!id) {
    throw new Error("requestData.id is not provided")
  }

  if (!token) {
    throw new Error("requestData.token is not provided")
  }

  fetch(`${host}/Customer/${id}/Cart`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application-json',
      'withCredentials': 'true',
      'Authorization': 'Bearer ' + token
    }
  })
  .then(response => {
    if (response.ok) {
      return response.json()
    }

    throw new Error("Failed loading cart contents")
  })
  .then(json => {
    onSuccess(json)
  })
  .catch(error => {
    onFail(error.toString())
  })
}

function ClearCartRequest(onSuccess, onFail, host, requestData) {
  if (!requestData) {
    throw new Error("Request data is not provided")
  }

  const { id, token } = requestData

  if (!id) {
    throw new Error("requestData.id is not provided")
  }

  if (!token) {
    throw new Error("requestData.token is not provided")
  }

  fetch(`${host}/Customer/${id}/Cart/Clear`, {
    method: 'DELETE',
    headers: {
      'Content-Type': 'application/json',
      'withCredentials': 'true',
      'Authorization': 'Bearer ' + token
    }
  })
  .then(response => {
    if (!response.ok) {
      throw new Error("Failed clearing cart")
    }

    onSuccess()
  })
  .catch(error => {
    onFail(error.toString())
  })
}


function DeleteFromCartRequest(onSuccess, onFail, host, requestData) {
  if (!requestData) {
    throw new Error("Request data is not provided")
  }

  const { id, token, data } = requestData

  if (!id) {
    throw new Error("requestData.id is not provided")
  }

  if (!token) {
    throw new Error("requestData.token is not provided")
  }
  
  if (!data) {
    throw new Error("requestData.data is not provided")
  }

  fetch(`${host}/Customer/${id}/Cart`, {
    method: 'DELETE',
    headers: {
      'Content-Type': 'application/json',
      'withCredentials': 'true',
      'Authorization': 'Bearer ' + token
    },
    body: JSON.stringify(data)
  })
  .then(response => {
    if (!response.ok) {
      throw new Error("Failed deleting cart position")
    }

    onSuccess()
  })
  .catch(error => {
    onFail(error.toString())
  })
}

function AddToCartRequest(onSuccess, onFail, host, requestData) {
  if (!requestData) {
    throw new Error("Request data is not provided")
  }

  const { id, token, data } = requestData

  if (!id) {
    throw new Error("requestData.id is not provided")
  }

  if (!token) {
    throw new Error("requestData.token is not provided")
  }
  
  if (!data) {
    throw new Error("requestData.data is not provided")
  }

  fetch(`${host}/Customer/${id}/Cart`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'withCredentials': 'true',
      'Authorization': 'Bearer ' + token
    },
    body: JSON.stringify(data)
  })
  .then(response => {
    if (!response.ok) {
      throw new Error("Failed adding product to cart")
    }

    return response.json()
  })
  .then(json => {
    onSuccess(json)
  })
  .catch(error => {
    onFail(error.toString())
  })
}

export function BuyRequest(onSuccess, onFail, host, requestData) {
  if (!requestData) {
    throw new Error("Request data is not provided")
  }

  const { id, token, data } = requestData

  if (!id) {
    throw new Error("requestData.id is not provided")
  }

  if (!token) {
    throw new Error("requestData.token is not provided")
  }

  if (!data) {
    throw new Error("requestData.data is not provided")
  }

  const { productId, count } = data
  

  fetch(`${host}/Products/${productId}/Order`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'withCredentials': 'true',
      'Authorization': 'Bearer ' + token
    },
    body: JSON.stringify({ userId: id, count })
  })
  .then(response => {
    if (response.ok) {
      return response.json()
    }

    throw new Error("Failed buying product")
  })
  .then(json => {
    onSuccess(json)
  })
  .catch(error => {
    onFail(error.toString())
  })
}

export function OrdersRequest(onSuccess, onFail, host, requestData) {
  if (!requestData) {
    throw new Error("Request data is not provided")
  }
  const { id, token } = requestData

  if (!id) {
    throw new Error("requestData.id is not provided")
  }
  if (!token) {
    throw new Error("requestData.token is not provided")
  }

  fetch(`${host}/Customer/${id}/Orders`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'withCredentials': 'true',
      'Authorization': 'Bearer ' + token
    }
  })
  .then(response => {
    if (response.ok) {
      return response.json()
    }

    throw new Error("Failed loading customer orders")
  })
  .then(json => {
    onSuccess(json)
  })
  .catch(error => {
    onFail(error.toString())
  })
}

function ShippingInfoFetchRequest(onSuccess, onFail, host, requestData) {
  if (!requestData) {
    throw new Error("Request data is not provided")
  }

  const { id, token } = requestData

  if (!id) {
    throw new Error("requestData.id is not provided")
  }
  if (!token) {
    throw new Error("requestData.token is not provided")
  }

  fetch(`${host}/Customer/${id}/ShippingInfo`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'withCredentials': 'true',
      'Authorization': 'Bearer ' + token
    }
  })
  .then(response => {
    if (response.ok) {
      const contentType = response.headers.get('Content-Type')
      if (contentType && contentType.includes('application/json')) {
        return response.json()
      }
      return
    }
    throw new Error("Failed loading shipping info")
  })
  .then(json => {
    onSuccess(json)
  })
  .catch(error => {
    onFail(error.toString())
  })

}

function ShippingInfoPostRequest(onSuccess, onFail, host, requestData) {
  if (!requestData) {
    throw new Error("Request data is not provided")
  }

  const { id, token, data } = requestData

  if (!id) {
    throw new Error("requestData.id is not provided")
  }
  if (!token) {
    throw new Error("requestData.token is not provided")
  }
  if (!data) {
    throw new Error("requestData.data is not provided")
  }

  fetch(`${host}/Customer/${id}/ShippingInfo`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'withCredentials': 'true',
      'Authorization': 'Bearer ' + token
    },
    body: JSON.stringify(data)
  })
  .then(response => {
    if (response.ok) {
      return response.json()
    }

    throw new Error("Failed saving shipping info")
  })
  .then(json => {
    onSuccess(json)
  })
  .catch(error => {
    onFail(error.toString())
  })
}

export const ShippingInfoRequest = {
  get: ShippingInfoFetchRequest,
  post: ShippingInfoPostRequest
}

export function SettingsRequest(onSuccess, onFail, host, requestData) {
  if (!requestData) {
    throw new Error("Request data is not provided")
  }

  const { id, token, data } = requestData

  if (!id) {
    throw new Error("requestData.id is not provided")
  }
  if (!token) {
    throw new Error("requestData.token is not provided")
  }
  if (!data) {
    throw new Error("requestData.data is not provided")
  }

  fetch(`${host}/Users/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      'withCredentials': 'true',
      'Authorization': 'Bearer ' + token
    },
    body: JSON.stringify(data)
  })
  .then(response => {
    if (response.ok) {
      return response.json()
    }

    throw new Error("Failed updating settings")
  })
  .then(json => {
    onSuccess(json.user, json.message)
  })
  .catch(error => {
    onFail(error.toString())
  })
}

export function ProductsRequest(onSuccess, onFail, host, url) {
  fetch(url ? url : `${host}/Products`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'withCredentials': true
    }
  })
    .then(response => {
      if (response.ok) {
        return response.json()
      }
      throw new Error("Failed loading products")
    })
    .then(json => {
      onSuccess(json)
    })
    .catch(error => {
      onFail(error.toString())
    })
}

export function SignInRequest({ login, password, role }, onSuccess, onFail, host) {
  const requestData = JSON.stringify({ login, password })

  switch (role) {
    case Role.Customer:
      fetch(`${host}/Auth/Customers/SignIn`, {
        method: 'POST',
        body: requestData,
        headers: {
          'Content-Type': 'application/json'
        }
      })
      .then(response => {
        if (response.ok) {
          return response.json()
        }
        throw new Error("Request failed: " + response.statusText + " " + response.statusText)
      })
      .then(json => {
        onSuccess({ ...json, role })
      })
      .catch(error => {
        onFail(error.toString())
      })
      break;
    case Role.Manager:
      fetch(`${host}/Auth/Managers/SignIn`, {
        method: 'POST',
        body: requestData,
        headers: {
          'Content-Type': 'application/json'
        }
      })
      .then(response => {
        if (response.ok) {
          return response.json()
        }
        throw new Error("Request failed: " + response.statusText + " " + response.statusText)
      })
      .then(json => {
        onSuccess({ ...json, role })
      })
      .catch(error => {
        onFail(error.toString())
      })
      break;
    default:
      console.log("Don't even think about it ;)")
  }
}

export function SignUpRequest({ firstName, lastName, email, phone, password, role }, onSuccess, onFail, host) {
  const requestData = JSON.stringify({ firstName, lastName, phone, email, password, role })

  switch (role) {
    case Role.Customer:
      fetch(`${host}/Auth/Customers/SignUp`, {
        method: 'POST',
        body: requestData,
        headers: {
          'Content-Type': 'application/json'
        }
      })
      .then(response => {
        if (response.ok) {
          return response.json()
        }
        throw new Error("Request failed: " + response.statusText + " " + response.statusText)
      })
      .then(json => {
        onSuccess({ ...json, role })
      })
      .catch(error => {
        onFail(error.toString())
      })
      break;
    case Role.Manager:
      fetch(`${host}/Auth/Managers/SignUp`, {
        method: 'POST',
        body: requestData,
        headers: {
          'Content-Type': 'application/json'
        }
      })
      .then(response => {
        if (response.ok) {
          return response.json()
        }
        throw new Error("Request failed: " + response.statusText + " " + response.statusText)
      })
      .then(json => {
        onSuccess({ ...json, role })
      })
      .catch(error => {
        onFail(error.toString())
      })
      break;
    default:
      console.log("Don't even think about it ;)")
  }
}