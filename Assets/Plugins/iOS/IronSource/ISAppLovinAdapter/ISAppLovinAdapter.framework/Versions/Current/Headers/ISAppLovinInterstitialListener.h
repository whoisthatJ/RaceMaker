//
//  ISAppLovinInterstitialListener.h
//  ISAppLovinAdapter
//
//  Created by Daniil Bystrov on 3/15/16.
//  Copyright © 2016 IronSource. All rights reserved.
//

#import <Foundation/Foundation.h>

#import <ALSdk.h>

@protocol ISAppLovinInterstitialDelegate <NSObject>
- (void)adUnitISDidLoadWithAd:(ALAd*)ad;
- (void)adUnitISDidFailToLoadWithError:(int)code;
- (void)adunitISWasDisplayed:(ALAd*)ad;
- (void)adunitISWasHidden:(ALAd*)ad;
- (void)adunitISWasClicked:(ALAd*)ad;
@end

@interface ISAppLovinInterstitialListener : NSObject <ALAdLoadDelegate, ALAdDisplayDelegate>
@property (nonatomic, weak) id<ISAppLovinInterstitialDelegate> delegate;
@end
