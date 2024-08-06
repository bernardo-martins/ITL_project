<template>
  <div class="container">
    <div class="row">
      <div class="col-12 text-center">
        <h3 class="pt-3">Shopping cart</h3>
      </div>
    </div>
    <!--    loop over all the cart items and display one by one-->
    <div
      v-for="cartItem in cartItems"
      :key="cartItem.course.courseId"
      class="row mt-2 pt-3 justify-content-around"
    >
      <!-- display course name and price -->
      <div class="col-md-5 px-3">
        <div class="card-block px-3">
          <h6 class="card-title">
            <router-link
              :to="{ name: 'Offers', params: { id: cartItem.course.courseId } }"
              >{{ cartItem.course.Name }}
            </router-link>
          </h6>
          <p id="item-price" class="mb-0 font-weight-bold">
            $ {{ cartItem.course.price }}
          </p>

          <!--remove from cart-->
          <br /><a href="#" class="text-right" @click="deleteItem(cartItem.id)"
            >Remove From Cart</a
          >
        </div>
      </div>
      <div class="col-2"></div>
      <div class="col-12"><hr /></div>
    </div>

    <!-- display total price -->
    <div class="total-cost pt-2 text-right">
      <h5>Total : € {{ totalcost.toFixed(2) }}</h5>
      <button
        :disabled="isDisabled()"
        type="button"
        class="btn btn-primary confirm"
        @click="checkout"
      >
        Confirm Order
      </button>
    </div>
  </div>
</template>

<!-- <script>
import axios from 'axios';
const axios = require('axios');
export default {
  data() {
    return {
      cartItems: [],
      price: 0,
    };
  },
  name: 'Cart',
  props: ['baseURL'],
  methods: {
    isDisabled() {
      return this.cartItems.length === 0;
    },
    // get all the cart items
    getCartItems() {
      axios
        .get(`${this.baseURL}/cart`)	
        .then((response) => {
          const result = response.data;
          this.cartItems = result.cartItems;
          this.price = result.price; 
        }
        )
        .catch((error) => {
          console.log(error);
        });


    },
    // delete item from cart
    deleteItem(CartItemId) {
      axios
        .delete(`${this.baseURL}/cart/${CartItemId}`)
        .then((response) => {
          this.getCartItems();
        })
        .catch((error) => {
          console.log(error);
        });
    },
    // checkout
    checkout() {
      this.$router.push({name :'/checkout'});
    },
    // check if cart is empty
    isDisabled() {
      return this.cartItems.length === 0;
    },
  },
  mounted() {
    this.getCartItems();
  },
};


</script> -->






<style scoped>
.Pay{
  display: flex;
  justify-content: center;
  margin-top: 20px;
}

</style>