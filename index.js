function App() {
  const baseUrl = "https://localhost:44352/Products/Images"
  const query = "?productId="
  const defaultProductId = 0

  const updateActionLink = function (requestForm, productId) {
    if (!requestForm) {
      throw new Error("Request form element was not provided")
    }

    requestForm.removeAttribute("action")

    requestForm.setAttribute("action", `${baseUrl}${query}${productId}`)
  }

  const initForm = function (formElement) {
    if (!formElement) {
      throw new Error("Form element was not provided!")
    }

    formElement.setAttribute("method", "post")
    formElement.setAttribute("enctype", "multipart/form-data")
    formElement.setAttribute("action", `${baseUrl}${query}${defaultProductId}`)
  }

  window.addEventListener('load', () => {
    const productIdInput = document.getElementById("product-id-input")
    const requestForm = document.getElementById("request-form")

    if (!productIdInput) {
      throw new Error("product-id-input not found!")
    }

    if (!requestForm) {
      throw new Error("request-form not found!")
    }

    initForm(requestForm)

    productIdInput.addEventListener("change", e => {
      let productId = parseInt(e.target.value)
      updateActionLink(requestForm, productId ? productId : defaultProductId)
    })
  })
}

App()
