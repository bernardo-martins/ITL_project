<template>
    <div class="div_class">
      <h3>You will be redirected to payment page after clicking BEZAHLEN</h3>
      <button
        class="checkout_button"
        id="proceed-to-checkout"
        @click="goToCheckout()"
      >
        Bezahlen 
      </button>
    </div>
  </template>


<!-- 
<script>
export default {
  data() {
    return {
      stripeAPIToken: process.env.vue_APP_STRIPETOKEN,
      stripe: '',
      token: null,
      sessionID: null,
      checkoutBodyArray: [],

    };
  },
  name: 'Checkout',
  props:['baseURL'],
  methods: {
    configureStripe() {},

    getALLCartItems() {
      axios.get(`${this.baseURL}/cart`)
        .then((response) => {
          if(response.data.length == 0)

          this.checkoutBodyArray.pusch({
            Name: course.cartItem.course.title,
            Price: course.cartItem.course.price,
          });
        })
        .catch((error) => {
          console.log(error);
        });
    },
    goToCheckout(){
      axios.post(
        this.baseURL + 'payment/Checkout-session',
        this.checkoutBodyArray
      )
      .then((response) => {
        localStrorage.setItem('sessionId', response.data.sessionId);
        sessionId: session.sessionId;
        })
      .then((session) => {
        return this.stripe.redirectToCheckout({
          sessionId: session.sessionId
        });
      });
  
    }

  },
   async mounted() {
    this.stripe = Stripe(this.stripeAPIToken);
    this.getALLCartItems();

  },
}; -->