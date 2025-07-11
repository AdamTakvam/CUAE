    
   /****************************************************************** 
    
       iLBC Speech Coder ANSI-C Source Code 
    
       enhancer.h 
     
    
    
    
       Copyright (c) 2001, 
       Global IP Sound AB. 
       All rights reserved. 
    
   ******************************************************************/ 
    
   #ifndef __ENHANCER_H 
   #define __ENHANCER_H 
    
   #include "iLBC_define.h" 
    
   float xCorrCoef(  
       float *target,      /* (i) first array */ 
       float *regressor,   /* (i) second array */ 
       int subl        /* (i) dimension arrays */ 
   ); 
    
   int enhancerInterface( 
       float *out,         /* (o) the enhanced recidual signal */ 
       float *in,          /* (i) the recidual signal to enhance */ 
       iLBC_Dec_Inst_t *iLBCdec_inst  
                           /* (i/o) the decoder state structure */ 
   ); 
    
   #endif 
    
    
